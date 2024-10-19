import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawTrnArbitrationComponent } from './law-trn-arbitration.component';

describe('LawTrnArbitrationComponent', () => {
  let component: LawTrnArbitrationComponent;
  let fixture: ComponentFixture<LawTrnArbitrationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawTrnArbitrationComponent]
    });
    fixture = TestBed.createComponent(LawTrnArbitrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
