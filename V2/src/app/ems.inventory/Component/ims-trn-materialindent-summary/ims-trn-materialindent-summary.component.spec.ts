import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMaterialindentSummaryComponent } from './ims-trn-materialindent-summary.component';

describe('ImsTrnMaterialindentSummaryComponent', () => {
  let component: ImsTrnMaterialindentSummaryComponent;
  let fixture: ComponentFixture<ImsTrnMaterialindentSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMaterialindentSummaryComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMaterialindentSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
