import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStocktransferlocationViewComponent } from './ims-trn-stocktransferlocation-view.component';

describe('ImsTrnStocktransferlocationViewComponent', () => {
  let component: ImsTrnStocktransferlocationViewComponent;
  let fixture: ComponentFixture<ImsTrnStocktransferlocationViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStocktransferlocationViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStocktransferlocationViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
