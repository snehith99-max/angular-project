import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnRaisematerialindentComponent } from './otl-trn-raisematerialindent.component';

describe('OtlTrnRaisematerialindentComponent', () => {
  let component: OtlTrnRaisematerialindentComponent;
  let fixture: ComponentFixture<OtlTrnRaisematerialindentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnRaisematerialindentComponent]
    });
    fixture = TestBed.createComponent(OtlTrnRaisematerialindentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
