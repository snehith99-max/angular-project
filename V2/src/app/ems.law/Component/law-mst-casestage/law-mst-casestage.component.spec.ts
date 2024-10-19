import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstCasestageComponent } from './law-mst-casestage.component';

describe('LawMstCasestageComponent', () => {
  let component: LawMstCasestageComponent;
  let fixture: ComponentFixture<LawMstCasestageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstCasestageComponent]
    });
    fixture = TestBed.createComponent(LawMstCasestageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
