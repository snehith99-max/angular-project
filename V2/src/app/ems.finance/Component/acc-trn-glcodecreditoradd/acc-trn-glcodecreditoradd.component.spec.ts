import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnGlcodecreditoraddComponent } from './acc-trn-glcodecreditoradd.component';

describe('AccTrnGlcodecreditoraddComponent', () => {
  let component: AccTrnGlcodecreditoraddComponent;
  let fixture: ComponentFixture<AccTrnGlcodecreditoraddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnGlcodecreditoraddComponent]
    });
    fixture = TestBed.createComponent(AccTrnGlcodecreditoraddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
