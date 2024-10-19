import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnMaterialindentAddComponent } from './ims-trn-materialindent-add.component';

describe('ImsTrnMaterialindentAddComponent', () => {
  let component: ImsTrnMaterialindentAddComponent;
  let fixture: ComponentFixture<ImsTrnMaterialindentAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnMaterialindentAddComponent]
    });
    fixture = TestBed.createComponent(ImsTrnMaterialindentAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
