import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccTrnGlcodedebitoraddComponent } from './acc-trn-glcodedebitoradd.component';

describe('AccTrnGlcodedebitoraddComponent', () => {
  let component: AccTrnGlcodedebitoraddComponent;
  let fixture: ComponentFixture<AccTrnGlcodedebitoraddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccTrnGlcodedebitoraddComponent]
    });
    fixture = TestBed.createComponent(AccTrnGlcodedebitoraddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
